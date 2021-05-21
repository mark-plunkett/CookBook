import React from 'react';
import { Container, Element } from 'react-bulma-components';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faSeedling } from '@fortawesome/free-solid-svg-icons'
import Icon from 'react-bulma-components/lib/components/icon';

export const Footer = props => {
    return (
        <Element className="pt-5">
            <Element className="has-background-white-bis">
                <Element style={{ width: '100%' }} className="has-text-centered my-5 py-5">
                    <Icon className="has-text-success" style={{ width: '100px', height: '100px' }}>
                        <FontAwesomeIcon icon={faSeedling} style={{ width: '100%', height: '100%' }} />
                    </Icon>
                </Element>
            </Element>
            <Container className="my-5">
                <Element className="has-text-right has-text-grey-light"><span>© CookBook {new Date().getFullYear()}</span></Element>
            </Container>
        </Element>
    );
}
