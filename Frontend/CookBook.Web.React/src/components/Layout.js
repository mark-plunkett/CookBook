import React, { Component } from 'react';
import { Header } from './Header';
import { Element, Section, Container } from 'react-bulma-components';
import { Footer } from './Footer';

export class Layout extends Component {
  static displayName = Layout.name;

  render() {
    return (
      <Element>
        <Header />
        <Element className="mt-5 pt-5">
          {this.props.children}
        </Element>
        <Footer />
      </Element>
    );
  }
}
