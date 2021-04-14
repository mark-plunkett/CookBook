import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Button, Form, Table } from 'react-bulma-components';
import { recipeStore } from '../models/recipes';

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

    static renderRecipesTable(recipes) {
        return (
            <Table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Title</th>
                        <th>Favourite</th>
                    </tr>
                </thead>
                <tbody>
                    {recipes.map(recipe =>
                        <tr key={recipe.title}>
                            <td>{recipe.title}</td>
                            <td>{recipe.isFavourite ? 'fav' : 'notfav'}</td>
                        </tr>
                    )}
                </tbody>
            </Table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Recipes.renderRecipesTable(this.state.recipes);

        return (
            <div>
                <h1 id="tabelLabel">Recipes</h1>
                <Button to="/recipes/create" renderAs={Link} className="is-link">Add Recipe</Button>
                {contents}
            </div>
        );
    }
}
