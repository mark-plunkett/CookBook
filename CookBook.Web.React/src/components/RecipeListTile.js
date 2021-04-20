import React from 'react';
import { Button, Card, Element } from 'react-bulma-components';
import { Link } from 'react-router-dom';
import Icon from 'react-bulma-components/lib/components/icon';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faEdit } from '@fortawesome/free-solid-svg-icons'
import { FavHeart } from './FavHeart';

export const RecipeListTile = (props) => {
    return (
        <Card>
            <Link to={'/recipes/view/' + props.recipe.id}>
                <Card.Image
                    size="square"
                    src={process.env.REACT_APP_API_URL + 'recipes/' + props.recipe.id + '/primaryimage?width=480&height=480'}
                    width={480}
                    height="auto">
                </Card.Image>
            </Link>
            <Card.Content className="">
                <p>{props.recipe.title}</p>
                <Element className="buttons is-right">
                    <FavHeart recipe={props.recipe} />
                </Element>
            </Card.Content>
        </Card>
    );
}
