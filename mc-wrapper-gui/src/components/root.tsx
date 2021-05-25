import { Route, Switch, useLocation } from 'react-router-dom';
import { Dashboard } from '../pages/dashboard/dashboard';
import React, { useEffect } from 'react';
import { CssBaseline } from '@material-ui/core';
import { useStyles } from './styles';
import Servers from './servers/servers';
import Drawer from './drawer/drawer';
import AppBar from './appbar/appbar';

export const Root = () => {
  const location = useLocation();
  const classes = useStyles();
  const [open, setOpen] = React.useState(true);
  const [title, setTitle] = React.useState('Dashboard');

  useEffect(() => {
    if (location.pathname.includes('/servers')) setTitle('Servers');
    else if (location.pathname.includes('/console')) setTitle('Console');
    else setTitle('Dashboard');
  });

  const handleDrawerOpen = () => setOpen(true);
  const handleDrawerClose = () => setOpen(false);

  return (
    <div className={classes.root}>
      <CssBaseline />

      <AppBar title={title} open={open} handleDrawerOpen={handleDrawerOpen} />

      <Drawer open={open} handleDrawerClose={handleDrawerClose} />

      <main className={classes.content}>
        <Switch>
          <Route exact path="/" component={Dashboard} />
          <Route exact path="/dashboard" component={Dashboard} />
          {/*<Route exact path="/console" component={Console} />*/}
          {/*<Route exact path="/plugins" component={Plugins} />*/}
          {/*<Route exact path="/configuration" component={Configuration} />*/}
          <Route exact path="/servers" component={Servers} />
        </Switch>
      </main>
    </div>
  );
};

export default Root;
