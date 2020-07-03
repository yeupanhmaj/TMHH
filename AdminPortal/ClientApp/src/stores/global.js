
import { Action, Reducer } from 'redux';
export const unloadedState = {
    token: localStorage.getItem('token') || undefined,
    name: localStorage.getItem('name') || undefined
};

export const Reducers= (state, incomingAction) => {
    if (state === undefined) {
        return unloadedState;
    }
    const action = incomingAction
    switch (action.type) {
        case 'USER_SET_DATA':
            return action.data;
    }

    return state;
};

export const actions = {

};



