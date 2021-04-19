import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Button, Columns, Level, Heading, Form } from 'react-bulma-components';
import { recipeStore } from '../models/recipes';
import { RecipeListTile } from './RecipeListTile';
import Icon from 'react-bulma-components/lib/components/icon';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faPlus } from '@fortawesome/free-solid-svg-icons'

const { Input, Field, Control } = Form;

export class Recipes extends Component {
    static displayName = Recipes.name;

    constructor(props) {
        super(props);
        this.state = { recipes: [], loading: true };
        this.setState = this.setState.bind(this)
        this.recipeSubscription$ = null;
    }

    async componentDidMount() {
        this.recipeSubscription$ = recipeStore.subscribe(this.setState);
        await recipeStore.init();
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
                <Level renderAs="nav">
                    <Level.Side align="left">
                        <Level.Item>
                            <Heading>
                                Recipes
                        </Heading>
                        </Level.Item>
                    </Level.Side>
                    <Level.Side align="right">
                        <Level.Item>
                            <Field kind="addons">
                                <Control>
                                    <Input placeholder="Find a recipe..."  value="" readOnly />
                                </Control>
                                <Control>
                                    <Button renderAs="button">
                                        Search
                                    </Button>
                                </Control>
                            </Field>
                        </Level.Item><Level.Item>
                            <Button to="/recipes/create" renderAs={Link} className="is-link">
                                <Icon>
                                    <FontAwesomeIcon icon={faPlus} />
                                </Icon>
                                <span>Add Recipe</span>
                            </Button>
                        </Level.Item>
                    </Level.Side>
                </Level>
                {contents}
            </div>
        );
    }
}