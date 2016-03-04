﻿/*
 * Attached to GameManager object 
 * 
 * Contains function for dealing cards
 * Instantiates Card Prefabs with CardObject components
 * Draws cards from Deck
 * 
 * Will eventually handle end of turns, signal opponent to play a card, etc.
 */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CardGame : MonoBehaviour {
	public static CardGame Instance;

    public CardPlayer player;
    public CardPlayer enemy;

    public Deck playerDeck; // Reference to Deck from Player Deck GameObject
    public Deck enemyDeck; // Reference to Deck from Enemy Deck GameObject

    public Transform[] playerHandTargets; // Array of transforms for each card in the player's hand
    public Transform[] enemyHandTargets; // Array of transforms for each card in enemy's hand

    public Transform[] enemyBoardTargets; // Where the enemy will place cards on the board

    public GameObject playerCardTemplate; // Reference to player card prefab
    public GameObject enemyCardTemplate; // Reference to enemy card prefab
    public GameObject cardCanvas; // Reference to canvas containing cards

    List<CardObject> playerCards; // Reference to cards from Player's CardPlayer
    List<CardObject> enemyCards; // Reference to cards from Enemy's CardPlayer

    Tuning tuning; // Reference to tuning object
    int numOfStartingCards; // Number of cards each card player starts with
    Vector3 cardScale;

	void Start () {
		Instance = this;

        tuning = Tuning.tuning;
        numOfStartingCards = tuning.numOfStartingCards;
        cardScale = tuning.cardScale;

        playerCards = player.GetCards();
        enemyCards = enemy.GetCards();

        DealCards(numOfStartingCards, playerDeck, playerHandTargets, player);
        DealCards(numOfStartingCards, enemyDeck, enemyHandTargets, enemy);

        showEnemyCard();
	}

    // This is called when the card game starts
	// Deals the number of starting cards to the player
    public void DealCards(int numOfCards, Deck deck, Transform[] handTargets, CardPlayer cardPlayer)
    {
        for (int i = 0; i < numOfCards; i++)
        {
            Transform currentTransform = handTargets[i];

            // Get cardInfo from next item in the deck
            CardInfo cardInfo = deck.DrawCard();

            // Get cardInfo's deck type
            DeckType cardDeckType = cardInfo.deckType;

            // Determine card template
            GameObject cardTemplate = null;
            if (cardDeckType == DeckType.Player)
            { // If this is a player card
                cardTemplate = playerCardTemplate; // Use player card template (that with more text fields for gold, salvage, etc.)
            }
            else if (cardDeckType == DeckType.AI)
            { // If this is an enemy card
                cardTemplate = enemyCardTemplate; // Use enemy card template
            }

            // Instantiate prefab with the current transform
            GameObject cardPrefab = (GameObject) Instantiate(cardTemplate, currentTransform.position, currentTransform.rotation);
            // Sets scale
			cardPrefab.transform.localScale = currentTransform.localScale;
			// Makes cardPrefab the child of the cardCanvas
			cardPrefab.transform.SetParent(cardCanvas.transform, false);

			// Retrieves CardObject component (either PlayerCardObject or EnemyCardObject)
            CardObject cardObject = cardPrefab.GetComponent<CardObject>();
            // Assign the CardInfo to this CardObject
            cardObject.CreateCard(cardInfo);
            // Give the CardObject reference to its CardPlayer
			cardObject.SetOwner(cardPlayer);
            // Add the CardObject to CardPlayer's hand
            cardPlayer.AddCardToHand(cardObject);
        }
    }

    // Temporary function
    // Makes enemy play first card and displays it on the board
    public void showEnemyCard()
    {
        CardObject card = enemyCards[0];
        card.transform.position = enemyBoardTargets[0].position;
		card.transform.localScale = enemyBoardTargets [0].localScale;
        enemy.PlayCard(card);
    }
}
