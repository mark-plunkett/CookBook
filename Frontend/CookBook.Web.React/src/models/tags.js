import { Subject } from "rxjs";
import { api } from "./api";

const fetchTags = async () => {
    const resp = await api.get("tags");
    return resp.data;
}

const subject = new Subject();

const initialState = {
    tags: [],
    loading: true,
    initialized: false
};

let state = initialState;

const initialize = async () => {
    state = {
        tags: await fetchTags(),
        loading: false,
        initialized: true
    };
}

export const tagStore = {
    init: async () => {
        if (!state.initialized)
            await initialize();

        subject.next(state);
    },
    subscribe: setState => subject.subscribe(setState),
    list: async () => {
        if (!state.initialized)
            await initialize();

        return state.tags;
    }
};
