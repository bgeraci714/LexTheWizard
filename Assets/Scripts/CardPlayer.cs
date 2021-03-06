﻿/*
 * Attached to all characters that participate in the card game.
 * 
 * Contains functions for getting cards list and adding/removing cards from the character's hand.
 * 
 * This script is referenced in CardGame.
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardPlayer : MonoBehaviour {
	
	protected GameController gameController;
	protected UIManager UImanager;
	protected Tuning tuning;
	
	protected string cardPlayerName; // Name of character
	protected List<CardObject> hand = new List<CardObject>(); // Reference to cards in hand
	protected Deck deck;
	
	protected virtual void Awake() {
		deck = GetComponent<Deck> ();
	}
	
	protected virtual void Start() {
		gameController = GameController.gameController;
		UImanager = UIManager.UImanager;
		tuning = Tuning.tuning;
	}
	
	public virtual void PlayCard(CardObject cardObject)
	{
		CardInfo playedCardInfo = cardObject.GetCardInfo();
		string cardName = cardObject.GetCardInfo().title;
		cardObject.PlayEffect ();
		RemoveCardFromHand(cardObject);
		
		string message = cardPlayerName + " just played " + cardName + ".";
        Debug.Log(message);
       // message += "\nIt has _____ effect.\nTap to read more about it!";
		message += cardObject.GetCardInfo().desc;
		UImanager.ShowPopup(message);
	}
	
	public Deck GetDeck() {
		return deck;
	}
	
	public List<CardObject> GetHand()
	{
		return hand;
	}
	
	public void AddCardToHand(CardObject cardObject)
	{
		hand.Add(cardObject);
	}
	
	public void RemoveCardFromHand(CardObject cardObject)
	{
		hand.Remove(cardObject);
		Debug.Log("Cards left in " + cardPlayerName + "\'s hand: " + hand.Count.ToString());
	}
	
	public void SetName(string name)
	{
		cardPlayerName = name;
	}
	
	public int NumberOfCardsInHand () {
		return hand.Count;
	}
}