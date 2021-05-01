import React from 'react';
import { Button } from 'react-bulma-components';
import Icon from 'react-bulma-components/lib/components/icon';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faHeart } from '@fortawesome/free-solid-svg-icons'
import { faHeart as farHeart } from '@fortawesome/free-regular-svg-icons'
import { favourite, unfavourite } from '../models/recipes';

export const FavHeart = (props) => {
    const onClick = _ => {
        const f = props.recipe.isFavourite
            ? unfavourite
            : favourite;
        f(props.recipe.id);
    }

    const isFav = props.recipe.isFavourite;
    const icon = isFav ? faHeart : farHeart;
    const style = isFav ? "" : "is-outlined";
    return (
        <Button className={'is-danger ' + style} onClick={onClick} >
            <Icon>
                <FontAwesomeIcon icon={icon} />
            </Icon>
        </Button>
    )
};