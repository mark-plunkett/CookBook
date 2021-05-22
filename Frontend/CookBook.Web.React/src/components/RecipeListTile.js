import React from 'react';
import { Card, Element } from 'react-bulma-components';
import { Link } from 'react-router-dom';
import { FavHeart } from './FavHeart';
import { TagList } from './TagList';

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
                <TagList tags={props.recipe.tags} allTags={props.allTags}></TagList>
            </Card.Content>
        </Card>
    );
}
