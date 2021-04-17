import React, { Component } from 'react';
import { Button, Card } from 'react-bulma-components';
import { Link } from 'react-router-dom';
import Icon from 'react-bulma-components/lib/components/icon';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faEdit } from '@fortawesome/free-solid-svg-icons'

export class RecipeListTile extends Component {
    static displayName = RecipeListTile.name;

    constructor(props) {
        super(props);
        this.state = props.recipe;
    }

    render() {
        return (
            <Card>
                <Card.Image
                    size="square"
                    src={process.env.REACT_APP_API_URL + 'recipes/' + this.state.id + '/primaryimage?width=480&height=480'}
                    width={480}
                    height="auto">

                </Card.Image>
                <Card.Content>
                    <p>{this.state.title}</p>
                    <Button to={'/recipes/edit/' + this.state.id} renderAs={Link} className="is-link is-small">
                        <Icon>
                            <FontAwesomeIcon icon={faEdit} />
                        </Icon>
                        <span>Edit</span>
                    </Button>
                </Card.Content>
            </Card>
        );
    }
}
