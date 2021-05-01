import React, { Component } from 'react';
import { Container, Heading, Hero } from 'react-bulma-components';
import { Link } from 'react-router-dom';

export class Header extends Component {
    render() {
        return (
            <Hero color="success" className="has-background-success-light">
                <Container>
                    <Link to={'/'}>
                        <Hero.Body className="">
                            <Heading size={1} className="has-text-grey-dark">
                                <span role="img" aria-label="food">🥗</span> Cookbook
                            </Heading>
                        </Hero.Body>
                    </Link>
                </Container>
            </Hero>
        )
    };
}