import { store } from '../stores';

export function openMessageDialog(errorHeader, errContent){
    store.dispatch({ type: 'MODAL_SET_LOADING_MODAL' , status : false });
    store.dispatch({ type: 'MODAL_OPEN_ERROR_MODAL', errHeader: errorHeader, errContent: errContent });
}


export function logout() {
    store.dispatch({ type: 'USER_SET_DATA', data: { token: '', name: '' } });
}


export function openPrintDialog(feature, id) {
    store.dispatch({ type: 'MODAL_OPEN_PRINT_MODAL', feature: feature, currentID: id });
}

export function closePrintDialog() {
    store.dispatch({ type: 'MODAL_CLOSE_PRINT_MODAL' });
}

export function setLoading(state) {
    store.dispatch({ type: 'MODAL_SET_LOADING_MODAL' , status : state });
}

