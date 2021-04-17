import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Button, Columns } from 'react-bulma-components';
import { recipeStore } from '../models/recipes';
import { RecipeListTile } from './RecipeListTile';

export class Recipes extends Component {
    static displayName = Recipes.name;


    constructor(props) {
        super(props);
        this.state = { recipes: [], loading: true };
        this.setState = this.setState.bind(this)
        this.recipeSubscription$ = null;
    }

    componentDidMount() {
        this.recipeSubscription$ = recipeStore.subscribe(this.setState);
        recipeStore.init(this.setState);
    }

    componentWillUnmount() {
        this.recipeSubscription$.unsubscribe();
    }

    static renderRecipes(recipes) {
        return (
            <Columns>
                {recipes.map(recipe =>
                    <Columns.Column key={recipe.id} size={3}>
                        <RecipeListTile recipe={recipe} />
                    </Columns.Column>
                )}
            </Columns>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Recipes.renderRecipes(this.state.recipes);

        return (
            <div>
                <h1 id="tabelLabel">Recipes</h1>
                <Button to="/recipes/create" renderAs={Link} className="is-link">Add Recipe</Button>
                {contents}
            </div>
        );
    }
}
