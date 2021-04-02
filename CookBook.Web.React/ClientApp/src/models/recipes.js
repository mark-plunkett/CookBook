import Axios from "axios";

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