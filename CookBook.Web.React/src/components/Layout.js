import React, { Component } from 'react';
import { NavMenu } from './NavMenu';
import { Header } from './Header';
import { Section, Container as BulmaContainer } from 'react-bulma-components';

export class Layout extends Component {
  static displayName = Layout.name;

  render() {
    return (
      <div>
        <Header />
        <Section>
          <BulmaContainer>
            {this.props.children}
          </BulmaContainer>
        </Section>
      </div>
    );
  }
}
