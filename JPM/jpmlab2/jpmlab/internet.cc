/*
 * internet.cc
 *
 *  Created on: Jun 19, 2019
 *      Author: Bart
 */


#include <string.h>
#include <omnetpp.h>

using namespace omnetpp;

class INTERNET : public cSimpleModule {
protected:
    // The following redefined virtual function holds the algorithm.
    virtual void initialize() override;
    virtual void handleMessage(cMessage *msg) override;
};

// The module class needs to be registered with OMNeT++
Define_Module(INTERNET);

void INTERNET::initialize() {}

void INTERNET::handleMessage(cMessage *msg) {}
