import clsx from 'clsx';
import {
  Drawer as DrawerMUI,
  Divider,
  IconButton,
  List,
  ListItem,
  ListItemIcon,
  ListItemText,
  ListSubheader,
} from '@material-ui/core';
import { Link } from 'react-router-dom';
import {
  Description as DescriptionIcon,
  Settings as SettingsIcon,
  ChevronLeft as ChevronLeftIcon,
  Dashboard as DashboardIcon,
  Power as PowerIcon,
  Storage as StorageIcon,
} from '@material-ui/icons';
import React from 'react';
import { useStyles } from './styles';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTerminal } from '@fortawesome/free-solid-svg-icons';

interface Props {
  open: boolean;
  handleDrawerClose: () => void;
}

const mainListItems = [
  { icon: <DashboardIcon />, text: 'Dashboard', link: '/dashboard' },
  {
    icon: <FontAwesomeIcon icon={faTerminal} />,
    text: 'Console',
    link: '/console',
  },
  { icon: <PowerIcon />, text: 'Plugins', link: '/plugins' },
  { icon: <SettingsIcon />, text: 'Configuration', link: '/configuration' },
  { icon: <StorageIcon />, text: 'Servers', link: '/servers' },
];

const secondaryListItems = [
  { icon: <DescriptionIcon />, text: 'Current month' },
  { icon: <DescriptionIcon />, text: 'Last quarter' },
  { icon: <DescriptionIcon />, text: 'Year-end sale' },
];

export const Drawer: React.FC<Props> = ({ open, handleDrawerClose }) => {
  const classes = useStyles();

  return (
    <DrawerMUI
      variant="permanent"
      classes={{
        paper: clsx(classes.drawerPaper, !open && classes.drawerPaperClose),
      }}
      open={open}>
      <div className={classes.toolbarIcon}>
        <IconButton onClick={handleDrawerClose}>
          <ChevronLeftIcon />
        </IconButton>
      </div>
      <Divider />
      <List>
        <div>
          {mainListItems.map((item, index) => (
            <Link
              to={item.link}
              key={index}
              style={{ color: 'inherit', textDecoration: 'inherit' }}>
              <ListItem button>
                <ListItemIcon>{item.icon}</ListItemIcon>
                <ListItemText primary={item.text} />
              </ListItem>
            </Link>
          ))}
        </div>
      </List>
      <Divider />
      <List>
        <div>
          <ListSubheader inset>Configs</ListSubheader>
          {secondaryListItems.map((item, index) => (
            <ListItem button key={index}>
              <ListItemIcon>{item.icon}</ListItemIcon>
              <ListItemText primary={item.text} />
            </ListItem>
          ))}
        </div>
      </List>
    </DrawerMUI>
  );
};

export default Drawer;
