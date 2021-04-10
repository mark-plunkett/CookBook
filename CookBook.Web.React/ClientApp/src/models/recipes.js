import Axios from "axios";
import { Subject } from "rxjs";
import { HubConnectionBuilder } from "@microsoft/signalr";

const api = Axios.create({
    baseURL: "/api"
});

export const getRecipes = async () => {
    const resp = await api.get("/recipes");
    return resp.data;
}

export const createRecipe = async (recipe) => {
    return await api.post("/recipes/create", recipe);
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
    }
};

const hub = new HubConnectionBuilder().withUrl("/recipeHub").build();
hub.on("RecieveRecipe", function (r) {
    recipeStore.appendRecipe(r);
});

hub.start().then(function () {
    //document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});