import clsx from 'clsx';
import {
  AppBar as AppBarMUI,
  Badge,
  IconButton,
  Toolbar,
  Typography,
} from '@material-ui/core';
import {
  Menu as MenuIcon,
  Notifications as NotificationsIcon,
} from '@material-ui/icons';
import React from 'react';
import { useStyles } from './styles';

interface Props {
  title: string;
  open: boolean;
  handleDrawerOpen: () => void;
}

export const AppBar: React.FC<Props> = ({ title, open, handleDrawerOpen }) => {
  const classes = useStyles();

  return (
    <AppBarMUI
      position="absolute"
      className={clsx(classes.appBar, open && classes.appBarShift)}>
      <Toolbar className={classes.toolbar}>
        <IconButton
          edge="start"
          color="inherit"
          aria-label="open drawer"
          onClick={handleDrawerOpen}
          className={clsx(
            classes.menuButton,
            open && classes.menuButtonHidden,
          )}>
          <MenuIcon />
        </IconButton>
        <Typography
          component="h1"
          variant="h6"
          color="inherit"
          noWrap
          className={classes.title}>
          {title}
        </Typography>
        <IconButton color="inherit">
          <Badge badgeContent={4} color="secondary">
            <NotificationsIcon />
          </Badge>
        </IconButton>
      </Toolbar>
    </AppBarMUI>
  );
};

export default AppBar;
