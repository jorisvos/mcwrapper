import {
  Box,
  CircularProgress,
  Container,
  Grid,
  Paper,
} from '@material-ui/core';
import clsx from 'clsx';
import { useStyles } from './styles';
import React, { useEffect } from 'react';
import { Copyright } from '../../components/copyright/copyright';
import Servers from '../../components/servers/servers';
import PieChart from '../../components/piechart/piechart';
import LineChart from '../../components/linechart/linechart';
import { CpuUsage, RamUsage } from '../../types/global';
import { GETCpuUsage, GETRamUsage } from '../../api/api';

export const Dashboard = () => {
  const classes = useStyles();
  const fixedHeightPaper = clsx(classes.paper, classes.fixedHeight);
  const [ramUsage, setRamUsage] = React.useState<RamUsage[]>();
  const [cpuUsage, setCpuUsage] = React.useState<CpuUsage[]>();

  useEffect(() => {
    if (ramUsage == undefined) setRamUsage(GETRamUsage());
    if (cpuUsage == undefined) setCpuUsage(GETCpuUsage());
  });

  return (
    <>
      <div className={classes.appBarSpacer} />
      <Container maxWidth="lg" className={classes.container}>
        <Grid container spacing={3}>
          {/* RAM Usage LineChart */}
          <Grid item xs={12} md={8} lg={9}>
            <Paper className={fixedHeightPaper}>
              {ramUsage ? (
                <LineChart
                  title="RAM Usage (in progress)"
                  info="RAM"
                  data={ramUsage}
                  unit="MB"
                />
              ) : (
                <CircularProgress />
              )}
            </Paper>
          </Grid>
          {/* CPU Usage PieChart */}
          <Grid item xs={12} md={4} lg={3}>
            <Paper className={fixedHeightPaper}>
              {cpuUsage ? (
                <PieChart title="CPU Usage (in progress)" data={cpuUsage} />
              ) : (
                <CircularProgress />
              )}
            </Paper>
          </Grid>
          {/* Recent Servers */}
          <Grid item xs={12}>
            <Paper className={classes.paper}>
              <Servers />
            </Paper>
          </Grid>
        </Grid>
        <Box pt={4}>
          <Copyright />
        </Box>
      </Container>
    </>
  );
};
