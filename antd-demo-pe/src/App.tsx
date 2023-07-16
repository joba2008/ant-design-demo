import React from 'react';
import { Carousel } from 'antd';

const contentStyle: React.CSSProperties = {
  height: '160px',
  color: '#fff',
  lineHeight: '160px',
  textAlign: 'center',
  background: '#364d79',
};

// set port=5500 && npm start

const App: React.FC = () => (
  <Carousel autoplay autoplaySpeed={15000}>
    <div>
      <iframe src="https://programiz.pro" name="iframe_target" width="100%" height="1070" loading="lazy" scrolling="no"></iframe>
    </div>
    <div>
      <iframe src="https://programiz.pro" name="iframe_target" width="100%" height="1070" loading="lazy" scrolling="no"></iframe>
    </div>
    <div>
      <iframe src="https://programiz.pro" name="iframe_target" width="100%" height="1070" loading="lazy" scrolling="no"></iframe>
    </div>
    <div>
      <iframe src="https://programiz.pro" name="iframe_target" width="100%" height="1070" loading="lazy" scrolling="no"></iframe>
    </div>
  </Carousel>
);

export default App;