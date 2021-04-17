import Axios from "axios";
import { Subject } from "rxjs";
import { HubConnectionBuilder } from "@microsoft/signalr";

const api = Axios.create({
    baseURL: process.env.REACT_APP_API_URL
});

export const getRecipes = async () => {
    console.log(api);
    const resp = await api.get("recipes");
    return resp.data;
}

export const createRecipe = async (recipe) => {
    return await api.post("recipes/create", recipe);
}

export const uploadFiles = async (files) => {
    const data = new FormData();
    for (var i = 0; i < files.length; i++) {
        data.append("files", files[i]);
    }

    const resp = await api.post("recipes/uploadfiles", data);
    return resp.data;
}

const subject = new Subject();

const initialState = {
    recipes: [],
    loading: true
};

let state = initialState;

export const recipeStore = {
    init: async () => {
        state = {
            recipes: await getRecipes(),
            loading: false
        };
        subject.next(state);
    },
    subscribe: setState => subject.subscribe(setState),
    appendRecipe: async (recipe) => {
        state = {
            recipes: [...state.recipes, recipe]
        }
        subject.next(state);
    },
    updateRecipe: async (recipe) => {
        state = {
            recipes: state.recipes.map(r => r.id === recipe.id ? recipe : r)
        }
        subject.next(state);
    }
};

const hub = new HubConnectionBuilder().withUrl(process.env.REACT_APP_API_URL + "recipeHub").build();
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