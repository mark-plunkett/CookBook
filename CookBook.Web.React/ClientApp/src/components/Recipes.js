import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { getRecipes } from '../models/recipes';

export class Recipes extends Component {
    static displayName = Recipes.name;

    constructor(props) {
        super(props);
        this.state = { recipes: [], loading: true };
    }

    componentDidMount() {
        this.fetchRecipes();
    }

    static renderRecipesTable(recipes) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Title</th>
                    </tr>
                </thead>
                <tbody>
                    {recipes.map(recipe =>
                        <tr key={recipe.title}>
                            <td>{recipe.title}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Recipes.renderRecipesTable(this.state.recipes);

        return (
            <div>
                <h1 id="tabelLabel" >Recipes</h1>
                <p>This component demonstrates fetching data from the server.</p>
                <Link to="/recipes/create">Create Recipe</Link>
                {contents}
            </div>
        );
    }

    async fetchRecipes() {
        const data = await getRecipes();
        this.setState({ recipes: data, loading: false });
    }
}
