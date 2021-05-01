import React from 'react';
import { Container } from 'react-bulma-components';

export const Footer = props => {
    return (
        <Container className="my-5">
            <div className="has-text-right has-text-grey-light"><span>© CookBook {new Date().getFullYear()}</span></div>
        </Container>
    );
}
