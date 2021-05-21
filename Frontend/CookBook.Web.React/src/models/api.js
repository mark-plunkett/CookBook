import Axios from "axios";
import { HubConnectionBuilder } from "@microsoft/signalr";

export const api = Axios.create({
    baseURL: process.env.REACT_APP_API_URL
});

export const signalRHub = new HubConnectionBuilder()
    .withUrl(process.env.REACT_APP_API_URL + "recipeHub")
    .withAutomaticReconnect()
    .build();