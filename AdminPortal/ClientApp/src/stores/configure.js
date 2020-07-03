import { connectRouter, routerMiddleware } from 'connected-react-router';

import { applyMiddleware, combineReducers, compose, createStore } from 'redux';
import thunk from 'redux-thunk';
import {  reducers } from '.';
import { loadTranslations, setLocale, syncTranslationWithStore } from 'react-redux-i18n';

const translationsObject = {
    en: {
      application: {
        title: 'Awesome app with i18n!',
        hello: 'Hello, %{name}!'
      },
      date: {
        long: 'MMMM Do, YYYY'
      },
      export: 'Export %{count} items',
      export_0: 'Nothing to export',
      export_1: 'Export %{count} item',
      two_lines: 'Line 1<br />Line 2',
      literal_two_lines: 'Line 1\
  Line 2'
    },
    nl: {
      application: {
        title: 'Toffe app met i18n!',
        hello: 'Hallo, %{name}!'
      },
      date: {
        long: 'D MMMM YYYY'
      },
      export: 'Exporteer %{count} dingen',
      export_0: 'Niks te exporteren',
      export_1: 'Exporteer %{count} ding',
      two_lines: 'Regel 1<br />Regel 2',
      literal_two_lines: 'Regel 1\
  Regel 2'
    }
  };
   
export default function configureStore(history, initialState) {
    const middleware = [
        thunk,
        routerMiddleware(history)
    ];
    const rootReducer = combineReducers({
        ...reducers,
        router: connectRouter(history)
    });

    const enhancers = [];
    const windowIfDefined = typeof window === 'undefined' ? null : window ;
    if (windowIfDefined && windowIfDefined.__REDUX_DEVTOOLS_EXTENSION__) {
        enhancers.push(windowIfDefined.__REDUX_DEVTOOLS_EXTENSION__());
    }
    const store = createStore(
        rootReducer,
        initialState,
        compose(applyMiddleware(...middleware), ...enhancers)
    );

    syncTranslationWithStore(store)
    store.dispatch(loadTranslations(translationsObject));
    store.dispatch(setLocale('en'));
    return store
}
