import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Button, Columns, Element, Level, Heading, Form, Tabs, Container } from 'react-bulma-components';
import { recipeStore } from '../models/recipes';
import { RecipeListTile } from './RecipeListTile';
import Icon from 'react-bulma-components/lib/components/icon';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faPlus, faSearch, faSeedling } from '@fortawesome/free-solid-svg-icons'
import _ from 'lodash';

const { Input, Field, Control } = Form;

export class Recipes extends Component {
    static displayName = Recipes.name;

    constructor(props) {
        super(props);
        this.sortOptions = {
            newest: {
                field: 'createdOn', order: 'desc'
            },
            name: {
                field: 'title', order: 'asc'
            }
        }
        this.state = {
            recipes: [],
            loading: true,
            orderBy: this.sortOptions.newest,
            search: ''
        };
        this.setState = this.setState.bind(this)
        this.handleChange = this.handleChange.bind(this);
        this.recipeSubscription$ = null;
    }

    async componentDidMount() {
        this.recipeSubscription$ = recipeStore.subscribe(state => this.setState({
            ...state,
            loading: state.loading,
            recipes: state.recipes
        }));
        await recipeStore.init();
    }

    componentWillUnmount() {
        this.recipeSubscription$.unsubscribe();
    }

    onTabSelected(field) {
        this.setState({
            ...this.state,
            orderBy: field
        });
    }

    handleChange(event) {
        const target = event.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        const name = target.name;
        this.setState({
            [name]: value
        });
    }

    renderRecipes(recipes) {
        const sorted =
            _.chain(recipes)
                .orderBy([this.state.orderBy.field], [this.state.orderBy.order])
                .filter(r => r.title.toLowerCase().includes(this.state.search.toLowerCase()))
                .value();
        return (
            <Columns className="mt-0 pt-5">
                {sorted.map(recipe =>
                    <Columns.Column key={recipe.id} size={3}>
                        <RecipeListTile key={recipe.id} recipe={recipe} />
                    </Columns.Column>
                )}
            </Columns>
        );
    }

    renderHeader() {
        return <Container className="pb-3">
            <Level renderAs="nav">
                <Level.Side align="left">
                    <Level.Item>
                        <Heading size={4}>
                            Recipes
                            </Heading>
                    </Level.Item>
                </Level.Side>
                <Level.Side align="right">
                    <Level.Item>
                        <Field kind="addons">
                            <Control iconRight>
                                <Input
                                    placeholder="Find a recipe..."
                                    value={this.state.search}
                                    name="search"
                                    onChange={this.handleChange} />
                                <Icon align="right">
                                    <FontAwesomeIcon icon={faSearch} className="is-success" />
                                </Icon>
                            </Control>
                        </Field>
                    </Level.Item><Level.Item>
                        <Button to="/recipes/create" renderAs={Link} className="is-success is-light">
                            <Icon>
                                <FontAwesomeIcon icon={faPlus} />
                            </Icon>
                            <span>Add Recipe</span>
                        </Button>
                    </Level.Item>
                </Level.Side>
            </Level>
        </Container>;
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderRecipes(this.state.recipes);

        return (
            <Element>
                {this.renderHeader()}
                <Element>
                    <Container>
                        <Tabs className="is-size-7">
                            <span className="is-italic mr-3">Sort by:</span>
                            <Tabs.Tab active={this.state.orderBy.field === this.sortOptions.newest.field} onClick={() => this.onTabSelected(this.sortOptions.newest)}>
                                Newest
                            </Tabs.Tab>
                            <Tabs.Tab active={this.state.orderBy.field === this.sortOptions.name.field} onClick={() => this.onTabSelected(this.sortOptions.name)}>
                                Name
                            </Tabs.Tab>
                        </Tabs>
                    </Container>
                    <Container>
                        {contents}
                    </Container>
                </Element>
            </Element>
        );
    }
}
