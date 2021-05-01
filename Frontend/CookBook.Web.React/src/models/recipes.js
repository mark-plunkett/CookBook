import Axios from "axios";
import { Subject } from "rxjs";
import { HubConnectionBuilder } from "@microsoft/signalr";

const api = Axios.create({
    baseURL: process.env.REACT_APP_API_URL
});

export const getRecipes = async () => {
    const resp = await api.get("recipes");
    return resp.data;
}

export const createRecipe = async (recipe) => {
    return await api.post("recipes", recipe);
}

export const updateRecipe = async (recipe) => {
    return await api.put("recipes", recipe);
}

export const uploadFiles = async (files) => {
    const data = new FormData();
    for (var i = 0; i < files.length; i++) {
        data.append("files", files[i]);
    }

    const resp = await api.post("recipes/uploadfiles", data);
    return resp.data;
}

export const favourite = async id => {
    return await api.patch("recipes/" + id + "/favourite");
}

export const unfavourite = async id => {
    return await api.patch("recipes/" + id + "/unfavourite");
}

const subject = new Subject();

const initialState = {
    recipes: [],
    loading: true,
    initialized: false
};

let state = initialState;

export const recipeStore = {
    init: async () => {
        if (!state.initialized)
            state = {
                recipes: await getRecipes(),
                loading: false,
                initialized: true
            };
        subject.next(state);
    },
    subscribe: setState => subject.subscribe(setState),
    appendRecipe: async (recipe) => {
        state = {
            ...state,
            recipes: [...state.recipes, recipe]
        }
        subject.next(state);
    },
    updateRecipe: async (recipe) => {
        state = {
            ...state,
            recipes: state.recipes.map(r => r.id === recipe.id ? recipe : r)
        }
        subject.next(state);
    },
    get: async id => {
        await recipeStore.init();
        return state.recipes.find(r => r.id === id);
    }
};

const hub = new HubConnectionBuilder()
    .withUrl(process.env.REACT_APP_API_URL + "recipeHub")
    .withAutomaticReconnect()
    .build();

hub.on("RecipeCreated", function (r) {
    recipeStore.appendRecipe(r);
});

hub.on("RecipeUpdated", function (r) {
    recipeStore.updateRecipe(r);
});

hub.start().then(function () {
    //document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});