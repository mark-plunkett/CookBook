import React, { Component } from 'react';
import { Button, Columns, Level, Element, Heading, Image } from 'react-bulma-components';
import { recipeStore } from '../models/recipes';
import { Link } from 'react-router-dom';
import Icon from 'react-bulma-components/lib/components/icon';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faEdit } from '@fortawesome/free-solid-svg-icons'
import { Container } from 'reactstrap';
import { FavHeart } from './FavHeart';
import { ParaSplitter } from './ParaSplitter';

export class ViewRecipe extends Component {

    constructor(props) {
        super(props);
        this.id = props.match.params.id;
        this.state = {
            title: '',
            description: '',
            instructions: '',
            ingredients: '',
            servings: 1,
            recipeAlbumDocumentID: null
        };
        this.recipeSubscription$ = null;
    }

    async componentDidMount() {
        this.recipeSubscription$ = recipeStore.subscribe(state => {
            var recipe = state.recipes.find(r => r.id === this.id);
            this.setState(recipe);
        });
        await recipeStore.init();
    }

    componentWillUnmount() {
        this.recipeSubscription$.unsubscribe();
    }

    getImage() {
        if (!!this.state.id)
            return (<Image
                size="square"
                src={process.env.REACT_APP_API_URL + 'recipes/' + this.state.id + '/primaryimage?width=480&height=480'}
                width={480}
                height="auto">
            </Image>);
    }

    render() {
        const fadedHR = {
            border: 0,
            height: '2px',
            backgroundImage: 'linear-gradient(to right, rgba(245,245,245,0.1), rgba(255,255,255,0.0))'
        }

        return (
            <Element>
                <Container>
                    <Level className="pt-5">
                        <Level.Side align="left">
                            <Heading size={4}>Recipe</Heading>
                        </Level.Side>
                        <Level.Side align="right" className="buttons">
                            <Button to={'/recipes/edit/' + this.state.id} renderAs={Link} className="is-success is-light">
                                <Icon>
                                    <FontAwesomeIcon icon={faEdit} />
                                </Icon>
                                <span>Edit</span>
                            </Button>
                            <FavHeart recipe={this.state} />
                        </Level.Side>
                    </Level>
                    <hr />
                    <Columns className="pt-5">
                        <Columns.Column size="two-thirds">
                            <Heading className="is-italic has-text-weight-normal" size={4}>{this.state.title}</Heading>
                            <hr style={fadedHR} />
                            <p size={5} className="pt-3">{this.state.description}</p>
                        </Columns.Column>
                        <Columns.Column size="one-quarter" offset={1}>
                            {this.getImage()}
                        </Columns.Column>
                    </Columns>
                    <Columns className="pt-3">
                        <Columns.Column size="two-thirds">
                            <Heading size={5} className="pb-3">How To:</Heading>
                            <ParaSplitter string={this.state.instructions} />
                        </Columns.Column>
                        <Columns.Column size="one-quarter" offset={1} className="has-background-light">
                            <Element className="has-text-centered">
                                <Heading size={6} className="mb-2">Ingredients</Heading>
                                <p className="is-size-7 has-text-grey">Serves {this.state.servings}</p>
                            </Element>
                            <Element className="px-3 has-text-grey">
                                <ParaSplitter string={this.state.ingredients} />
                            </Element>
                        </Columns.Column>
                    </Columns>
                </Container>
            </Element>
        );
    }
}