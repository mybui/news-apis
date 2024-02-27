import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  render() {
    return (
      <div style={{paddingLeft: '50px', paddingTop: '50px'}}>
        <a href='https://localhost:44495/news' style={{color: 'blue', fontSize: '2em'}}>News APIs</a>
      </div>
    );
  }
}