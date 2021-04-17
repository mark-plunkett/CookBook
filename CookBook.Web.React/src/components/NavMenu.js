import React, { Component } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import { Container, Heading, Hero } from 'react-bulma-components';
//import Heading from 'react-bulma-components/lib/components/heading';
//import Section from 'react-bulma-components/lib/components/section';
//import Container from 'react-bulma-components/lib/components/container';

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

    //  render () {
    //    return (
    //      <header>
    //        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
    //          <Container>
    //            <NavbarBrand tag={Link} to="/">CookBook</NavbarBrand>
    //            <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
    //            <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
    //              <ul className="navbar-nav flex-grow">
    //                <NavItem>
    //                  <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
    //                </NavItem>
    //              </ul>
    //            </Collapse>
    //          </Container>
    //        </Navbar>
    //      </header>
    //    );
    //  }

}
