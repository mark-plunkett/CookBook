import React, { Component } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import { Container, Heading, Hero } from 'react-bulma-components';

export class NavMenu extends Component {
    static displayName = NavMenu.name;

    constructor(props) {
        super(props);

        this.toggleNavbar = this.toggleNavbar.bind(this);
        this.state = {
            collapsed: true
        };
    }

    toggleNavbar() {
        this.setState({
            collapsed: !this.state.collapsed
        });
    }

    render() {
        return (
            <Container>
                <Hero color="primary">
                    <Hero.Body>
                        <Heading size={1}>
                            Cook Book
                        </Heading>
                        <Heading subtitle size={6}>
                            Yummy recipes
                        </Heading>
                    </Hero.Body>
                </Hero>
            </Container>
        )
    };

}
