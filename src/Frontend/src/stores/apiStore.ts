import {configureStore} from "@reduxjs/toolkit";
import {usersApi} from "../features/users/usersApi.ts";

export const apiStore = configureStore({
    reducer: {
        [usersApi.reducerPath]: usersApi.reducer,
    },
    middleware: (getDefaultMiddleware) =>
        getDefaultMiddleware().concat(usersApi.middleware),
});

export type RootState = ReturnType<typeof apiStore.getState>;
export type AppDispatch = typeof apiStore.dispatch;