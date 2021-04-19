import React, { Component } from 'react';
import { Container, Heading, Hero, Section } from 'react-bulma-components';

export class Header extends Component {
    render() {
        return (
            <Section>
                <Container>
                    <Hero color="warning" gradient>
                        <Hero.Body className="has-text-centered">
                            <Heading size={1}>
                                Cook Book
                        </Heading>
                            <Heading subtitle size={6}>
                                Yummy recipes
                        </Heading>
                        </Hero.Body>
                    </Hero>
                </Container>
            </Section>
        )
    };
}