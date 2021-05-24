import React from 'react';
import ReactDOM from 'react-dom';
import { ThemeProvider } from '@material-ui/core/styles';
import { CssBaseline } from '@material-ui/core';
import theme from './core/theme';
import { BrowserRouter, Switch, Route } from 'react-router-dom';
import Root from './components/root';

ReactDOM.render(
  <ThemeProvider theme={theme}>
    {/* CssBaseline kickstart an elegant, consistent, and simple baseline to build upon. */}
    <CssBaseline />
    <BrowserRouter>
      <Root />
    </BrowserRouter>
  </ThemeProvider>,
  document.querySelector('#root'),
);
