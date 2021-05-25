import React, { useEffect } from 'react';
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow,
  CircularProgress,
  Link,
} from '@material-ui/core';
import Title from '../title/title';
import { GETAllServerInfo } from '../../api/api';
import { ServerInfo } from '../../types/global';
import { useStyles } from './styles';

export const Servers = () => {
  const classes = useStyles();
  const [servers, setServers] = React.useState<ServerInfo[]>();

  useEffect(() => {
    if (servers == undefined)
      GETAllServerInfo(5).then((data) => setServers(data));
  });

  return (
    <React.Fragment>
      <Title>Servers</Title>
      {servers ? (
        <Table size="small">
          <TableHead>
            <TableRow>
              <TableCell>Name</TableCell>
              <TableCell>Plugins enabled</TableCell>
              <TableCell>Created at</TableCell>
              <TableCell>Status</TableCell>
              <TableCell align="right">Id</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {servers.map((server) => (
              <TableRow key={server.id}>
                <TableCell>{server.name}</TableCell>
                <TableCell>{server.enablePlugins.toString()}</TableCell>
                <TableCell>{server.createdAt}</TableCell>
                <TableCell>{server.isRunning.toString()}</TableCell>
                <TableCell align="right">{server.id}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      ) : (
        <CircularProgress />
      )}
      <div className={classes.seeMore}>
        <Link color="primary" href="/servers">
          See all servers
        </Link>
      </div>
    </React.Fragment>
  );
};

export default Servers;
