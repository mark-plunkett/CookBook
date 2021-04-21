import React, { Component } from 'react';
import { Header } from './Header';
import { Element, Section, Container as BulmaContainer } from 'react-bulma-components';

export class Layout extends Component {
  static displayName = Layout.name;

  render() {
    return (
      <Element>
        <Header />
        <Section className="px-0">
          {this.props.children}
        </Section>
        <Element></Element>
      </Element>
    );
  }
}
