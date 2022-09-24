import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux'

import { setupStore } from './app/store';
import App from './app';
import reportWebVitals from './reportWebVitals';


const store = setupStore();

ReactDOM.render(
  <React.StrictMode>
    <Provider store={store}>
      <App />
    </Provider>
  </React.StrictMode>,
  document.getElementById('root')
);

reportWebVitals();
