import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';

import './custom.css'
import { Recipes } from './components/Recipes';
import { CreateRecipe } from './components/CreateRecipe';
import { EditRecipe } from './components/EditRecipe';
import { ViewRecipe } from './components/ViewRecipe';

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <Layout>
                <Route exact path='/' component={Recipes} />
                <Route path='/recipes/create' component={CreateRecipe} />
                <Route path='/recipes/edit/:id' component={EditRecipe} />
                <Route path='/recipes/view/:id' component={ViewRecipe} />
            </Layout>
        );
    }
}
