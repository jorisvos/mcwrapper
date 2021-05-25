import React from 'react';
import { useTheme } from '@material-ui/core/styles';
import {
  LineChart as Chart,
  Line,
  XAxis,
  YAxis,
  Label,
  ResponsiveContainer,
} from 'recharts';
import Title from '../title/title';

interface Props {
  title: string;
  info: string;
  data: {
    timestamp: string;
    amount: number;
  }[];
  unit: string;
}

export const LineChart: React.FC<Props> = ({ title, info, data, unit }) => {
  const theme = useTheme();

  return (
    <React.Fragment>
      <Title>{title}</Title>
      <ResponsiveContainer>
        <Chart
          data={data}
          margin={{
            top: 16,
            right: 16,
            bottom: 0,
            left: 24,
          }}>
          <XAxis dataKey="timestamp" stroke={theme.palette.text.secondary} />
          <YAxis unit={unit} stroke={theme.palette.text.secondary}>
            <Label
              angle={270}
              position="left"
              style={{
                textAnchor: 'middle',
                fill: theme.palette.text.primary,
              }}
              dx={-13}>
              {info}
            </Label>
          </YAxis>
          <Line
            type="monotone"
            dataKey="amount"
            stroke={theme.palette.primary.main}
            dot={false}
          />
        </Chart>
      </ResponsiveContainer>
    </React.Fragment>
  );
};

export default LineChart;
