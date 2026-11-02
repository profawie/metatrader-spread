package com.smc.mt4priceapi.config;

import com.smc.mt4priceapi.service.Mt4PriceService;

import org.springframework.amqp.core.Queue;
import org.springframework.amqp.rabbit.connection.ConnectionFactory;
import org.springframework.amqp.rabbit.listener.MessageListenerContainer;
import org.springframework.amqp.rabbit.listener.SimpleMessageListenerContainer;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class RabbitmqConfig {

    @Value("${account.code}")
    private String accountCode;

    @Autowired
    private Mt4PriceService mt4PriceService;

    public String getQueueName() {
        return "MT4Price_" + accountCode + "_Queue";
    }

    @Bean
    public Queue queue() {
        return new Queue(getQueueName(), false);
    }


	@Bean
	MessageListenerContainer messageListenerContainer(ConnectionFactory connectionFactory ) {
		SimpleMessageListenerContainer simpleMessageListenerContainer = new SimpleMessageListenerContainer();
		simpleMessageListenerContainer.setConnectionFactory(connectionFactory);
		simpleMessageListenerContainer.setQueues(queue());
		simpleMessageListenerContainer.setMessageListener(mt4PriceService);
		return simpleMessageListenerContainer;

	}
    
}
