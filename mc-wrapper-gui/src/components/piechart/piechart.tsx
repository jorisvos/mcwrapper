import React from 'react';
import Title from '../title/title';
import {
  PieChart as Chart,
  Pie,
  Tooltip,
  Cell,
  ResponsiveContainer,
} from 'recharts';

const COLORS = ['#0088FE', '#00C49F', '#FFBB28', '#FF8042'];

interface Props {
  title: string;
  data: {
    name: string;
    value: number;
  }[];
}

export const PieChart: React.FC<Props> = ({ title, data }) => {
  return (
    <React.Fragment>
      <Title>{title}</Title>
      <ResponsiveContainer width="100%" height="100%">
        <Chart width={400} height={400}>
          <Pie
            data={data}
            cx="50%"
            cy="50%"
            innerRadius={60}
            outerRadius={80}
            fill="#8884d8"
            paddingAngle={5}
            dataKey="value">
            {data.map((entry, index) => (
              <Cell
                key={`cell-${index}`}
                fill={COLORS[index % COLORS.length]}
              />
            ))}
          </Pie>
          <Tooltip formatter={(value: any) => `${value}%`} />
        </Chart>
      </ResponsiveContainer>
    </React.Fragment>
  );
};

export default PieChart;
