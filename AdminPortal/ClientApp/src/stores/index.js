


import * as MODAL from './modals';
import * as GLOBAL from './global';
import * as PRINT from './print';
import history from './history';

// export interface ApplicationState {
//     [key]: Object | undefined
// }
// Whenever an action is dispatched, Redux will update each top-level application state property using
// the reducer with the matching name. It's important that the names match exactly, and that the reducer
// acts on the corresponding ApplicationState property type.

export const reducers = {
    modal: MODAL.Reducers ,
    global: GLOBAL.Reducers,
    print: PRINT.Reducers,
};

// This type can be used as a hint on action creators so that its 'dispatch' and 'getState' params are
// correctly typed to match your store.

let temp = require('./configure').default

export const store = temp(history);