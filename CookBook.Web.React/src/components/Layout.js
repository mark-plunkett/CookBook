import React, { Component } from 'react';
import { Header } from './Header';
import { Element, Section, Container as BulmaContainer } from 'react-bulma-components';

export class Layout extends Component {
  static displayName = Layout.name;

  render() {
    return (
      <Element>
        <Header />
        <Section>
          <BulmaContainer>
            <hr />
            {this.props.children}
          </BulmaContainer>
        </Section>
      </Element>
    );
  }
}
