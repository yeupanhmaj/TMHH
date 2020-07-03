
export const unloadedState = {
    feature: '',
    currentID: 0,
    printModal: false
};

export const actions = {
    print: (feature, currentID) => (dispatch, getState) => {
        dispatch({ type: 'MODAL_OPEN_PRINT_MODAL', feature: feature, currentID: currentID });
    },
    closePrint: () => (dispatch, getState) => {
        dispatch({ type: 'MODAL_CLOSE_PRINT_MODAL' });
    },
};
export const Reducers = (state , incomingAction) => {
    if (state === undefined) {
        return unloadedState;
    }
    const action = incomingAction ;
    switch (action.type) {
        case 'MODAL_OPEN_PRINT_MODAL':             
            return {
                currentID: action.currentID,
                feature: action.feature,
                printModal: true,
            };
        case 'MODAL_CLOSE_PRINT_MODAL':
            return {
                currentID: 0,
                feature: '',
                printModal: false
            }
    }
    return state;
};