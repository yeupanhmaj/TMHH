

export const unloadedState = {
    errHeader: '',
    errContent: '',
    loadingModal: false,
    errModal: false
};
export const actions = {
    error: (errHeader, errContent) => (dispatch, getState) => {
        dispatch({ type: 'MODAL_OPEN_ERROR_MODAL', errHeader: errHeader, errContent: errContent });
    },
    closeError: () => (dispatch, getState) => {
        dispatch({ type: 'MODAL_CLOSE_ERROR_MODAL' });
    },
    loading: (loadingStatus) => (dispatch, getState) => {
        dispatch({ type: 'MODAL_SET_LOADING_MODAL', status: loadingStatus });
    },
};
export const Reducers = (state, incomingAction) => {
    if (state === undefined) {
        return unloadedState;
    }
    const action = incomingAction;

    switch (action.type) {
        case 'MODAL_OPEN_ERROR_MODAL':
        return {
            ...state,
            errHeader: action.errHeader,
            errContent : action.errContent,
            errModal: true,
            loadingModal:false
          };
        case 'MODAL_CLOSE_ERROR_MODAL':
        return {
            ...state,        
            errModal: false
          };
        case 'MODAL_SET_LOADING_MODAL':
        return {
            ...state,        
            loadingModal: action.status
          };
    }
    return state;
};