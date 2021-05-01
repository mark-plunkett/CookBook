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
        this.state = {
            recipes: [],
            loading: true,
            orderBy: 'createdOn',
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
                .sortBy([this.state.orderBy])
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
                            <Tabs.Tab active={this.state.orderBy === 'createdOn'} onClick={() => this.onTabSelected('createdOn')}>
                                Date Added
                            </Tabs.Tab>
                            <Tabs.Tab active={this.state.orderBy === 'title'} onClick={() => this.onTabSelected('title')}>
                                Name
                            </Tabs.Tab>
                        </Tabs>
                    </Container>
                    <Element className="has-background-white-bis">
                        <Container>
                            {contents}
                            <Element style={{ width: '100%' }} className="has-text-centered my-5 py-5">
                                <Icon className="has-text-success" style={{ width: '100px', height: '100px' }}>
                                    <FontAwesomeIcon icon={faSeedling} style={{ width: '100%', height: '100%' }} />
                                </Icon>
                            </Element>
                        </Container>
                    </Element>
                </Element>
            </Element>
        );
    }
}
