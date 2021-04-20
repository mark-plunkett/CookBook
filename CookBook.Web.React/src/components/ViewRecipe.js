import React, { Component } from 'react';
import { Buttons, Button, Columns, Level, Element, Heading, Section, Image } from 'react-bulma-components';
import { recipeStore } from '../models/recipes';
import { Link } from 'react-router-dom';
import Icon from 'react-bulma-components/lib/components/icon';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faEdit } from '@fortawesome/free-solid-svg-icons'
import { Container } from 'reactstrap';
import { FavHeart } from './FavHeart';

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
        return (
            <Element>
                <Container>
                    <Level className="pt-5">
                        <Level.Side align="left">
                            <h3>Recipe</h3>
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
                            <hr style={{ width: '50%' }} />
                            <p size={5} className="pt-3">{this.state.description}</p>
                        </Columns.Column>
                        <Columns.Column size="one-quarter" offset={1}>
                            {this.getImage()}
                        </Columns.Column>
                    </Columns>
                    <Columns className="pt-3">
                        <Columns.Column size="two-thirds">
                            <h3 className="pb-3">How To:</h3>
                            {this.state.instructions.split('\n').map((p, idx) => <p key={idx}>{p}</p>)}
                        </Columns.Column>
                        <Columns.Column size="one-quarter" offset={1} className="has-background-light">
                            <Element className="has-text-centered">
                                <h5>Ingredients</h5>
                                <p className="is-size-7 has-text-grey">Serves {this.state.servings}</p>
                            </Element>
                            <Element className="px-3 has-text-grey">
                                {this.state.ingredients.split('\n').map((p, idx) => <p key={idx}>{p}</p>)}
                            </Element>
                        </Columns.Column>
                    </Columns>

                </Container>
            </Element>
        );
    }
}