import React, { Component } from 'react';
import { Button } from 'react-bulma-components';
import Icon from 'react-bulma-components/lib/components/icon';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faHeart } from '@fortawesome/free-solid-svg-icons'
import { faHeart as farHeart } from '@fortawesome/free-regular-svg-icons'
import { favourite, unfavourite } from '../models/recipes';
import { recipeStore } from '../models/recipes';

export class FavHeart extends Component {

    constructor(props) {
        super(props);
        this.state = props.recipe;
        this.setState = this.setState.bind(this)
        this.recipeSubscription$ = null;
    }

    componentDidMount() {
        this.recipeSubscription$ = recipeStore.subscribe(state => {
            var recipe = state.recipes.find(r => r.id === this.state.id);
            this.setState(recipe);
        });
    }

    componentWillUnmount() {
        this.recipeSubscription$.unsubscribe();
    }

    onClick = e => {
        if (this.state.isFavourite)
            unfavourite(this.state.id);
        else
            favourite(this.state.id);
    }

    render() {
        const isFav = this.state.isFavourite;
        const icon = isFav ? faHeart : farHeart;
        const style = isFav ? "" : "is-outlined";
        return (
            <Button className={'is-danger ' + style} onClick={this.onClick} >
                <Icon>
                    <FontAwesomeIcon icon={icon} />
                </Icon>
            </Button>
        )
    }
};