import React, { Component } from 'react';
import { Container, Heading, Hero, Section } from 'react-bulma-components';
import { Link } from 'react-router-dom';

export class Header extends Component {
    render() {
        return (
            <Container>
                <Hero color="warning" gradient>
                    <Link to={'/'}>
                        <Hero.Body className="has-text-centered">
                            <Heading size={1}>
                                Cook Book
                        </Heading>
                            <Heading subtitle size={6}>
                                Yummy recipes
                        </Heading>
                        </Hero.Body>
                    </Link>
                </Hero>
            </Container>
        )
    };
}