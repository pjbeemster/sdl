package com.sdl.smarttarget.extensions.deployment;

/*
 * Copyright (c) 2016 SDL Group.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
 */

import com.tridion.configuration.Configuration;
import com.tridion.configuration.ConfigurationException;
import com.tridion.deployer.ProcessingException;
import com.tridion.deployer.Processor;
import com.tridion.smarttarget.Logger;
import com.tridion.transport.transportpackage.TransportPackage;

public class FredhopperDeployerModuleWrapper extends com.tridion.smarttarget.deployer.FredhopperDeployerModule {

    public FredhopperDeployerModuleWrapper(Configuration config, Processor processor) throws ConfigurationException {
        super(config, processor);
    }

    @Override
    public void process(TransportPackage data) {
        try {

            super.process(data);

        } catch(ProcessingException e) {

            Logger.warn("FredhopperDeployerModuleWrapper", e.getMessage());

            // handle exception
            // rethrow if anything other than Fredhopper Indexer unreachable

        }
    }
}

