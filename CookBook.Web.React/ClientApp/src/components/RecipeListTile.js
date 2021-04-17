import React, { Component } from 'react';
import { Card } from 'react-bulma-components';

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
                    src={'/api/recipes/' + this.state.id + '/primaryimage?width=480&height=480'}
                    width={480}
                    height="auto">

                </Card.Image>
                <Card.Content>
                    {this.state.title}
                </Card.Content>
            </Card>
        );
    }
}
